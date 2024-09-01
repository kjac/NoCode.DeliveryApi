using Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.Indexing;

public sealed class NoCodeContentIndexer : IContentIndexHandler
{
    private readonly IFilterService _filterService;
    private readonly ISortService _sortService;
    private readonly IFieldBufferService _fieldBufferService;
    private readonly ILogger<NoCodeContentIndexer> _logger;
    private readonly IDictionary<string, IPropertyTypeParser> _propertyTypeParsers;
    private readonly IPropertyTypeParser _fallbackPropertyTypeParser;
    private readonly IContentTypeService _contentTypeService;

    [Obsolete("Use the constructor that accepts IContentTypeService. Will be removed in V2.")]
    public NoCodeContentIndexer(
        IFilterService filterService,
        ISortService sortService,
        IFieldBufferService fieldBufferService,
        ILogger<NoCodeContentIndexer> logger,
        IJsonSerializer jsonSerializer)
        : this(filterService, sortService, fieldBufferService, logger, jsonSerializer, StaticServiceProvider.Instance.GetRequiredService<IContentTypeService>())
    {
    }

    public NoCodeContentIndexer(
        IFilterService filterService,
        ISortService sortService,
        IFieldBufferService fieldBufferService,
        ILogger<NoCodeContentIndexer> logger,
        IJsonSerializer jsonSerializer,
        IContentTypeService contentTypeService)
    {
        _filterService = filterService;
        _sortService = sortService;
        _fieldBufferService = fieldBufferService;
        _logger = logger;
        _contentTypeService = contentTypeService;

        // content index handlers are singletons, so this is OK
        _propertyTypeParsers = new Dictionary<string, IPropertyTypeParser>
        {
            { Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.ContentPicker, new ContentPickerParser() },
            { Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.MediaPicker3, new MediaPickerParser(jsonSerializer) },
            { Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.Tags, new TagsParser(jsonSerializer) },
            { Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.MultipleTextstring, new MultipleTextStringParser() },
            { Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.CheckBoxList, new CheckBoxListParser(jsonSerializer) },
            { Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.DropDownListFlexible, new DropDownListParser(jsonSerializer) },
            { Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.ColorPicker, new ColorPickerParser(jsonSerializer) },
            { Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.Slider, new SliderParser() },
            { Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.Integer, new IntegerParser() },
            { Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.Decimal, new DecimalParser() },
            { Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.DateTime, new DateTimeParser() },
            { Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.Boolean, new BooleanParser() },
            { Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.MultiNodeTreePicker, new MultiNodeTreePickerParser() },
            { Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.MultiUrlPicker, new MultiUrlPickerParser(jsonSerializer) },
            { Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.MarkdownEditor, new MarkdownParser() },
            { Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.ImageCropper, new ImageCropperParser(jsonSerializer) },
            { Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.TinyMce, new RichTextParser(jsonSerializer, _logger) },
        };
        _fallbackPropertyTypeParser = new FallbackParser();
    }

    public IEnumerable<IndexFieldValue> GetFieldValues(IContent content, string? culture)
    {
        var indexFieldValues = FilterIndexFieldValues(content, culture)
            .Union(SortIndexFieldValues(content, culture))
            .ToList();

        return indexFieldValues;
    }

    public IEnumerable<IndexField> GetFields()
    {
        IEnumerable<FilterModel> filters = _filterService.GetAllAsync().GetAwaiter().GetResult();
        IEnumerable<SortModel> sorts = _sortService.GetAllAsync().GetAwaiter().GetResult();
        IEnumerable<IIndexFieldModel> buffer = _fieldBufferService.AvailableFields();

        return filters.OfType<IIndexFieldModel>().Union(sorts).Union(buffer).Select(field => new IndexField
        {
            FieldName = field.IndexFieldName,
            FieldType = field.IndexFieldType,
            // at this point, VariesByCulture has no actual use in Core :|
            VariesByCulture = false
        }).ToArray();
    }

    private List<IndexFieldValue> FilterIndexFieldValues(IContent content, string? culture)
    {
        var indexFieldValues = new List<IndexFieldValue>();
        IEnumerable<FilterModel> filters = _filterService.GetAllAsync().GetAwaiter().GetResult();
        IContentType contentType = _contentTypeService.Get(content.ContentTypeId)
                                   ?? throw new InvalidOperationException($"The content type with ID {content.ContentTypeId} ({content.ContentType.Alias}) could not be found.");
        IPropertyType[] propertyTypes = contentType.CompositionPropertyTypes.ToArray();

        foreach (FilterModel filter in filters)
        {
            // filters can have multiple values per content item
            var indexValues = new List<object>();
            foreach (var fieldAlias in filter.PropertyAliases)
            {
                if (content.HasProperty(fieldAlias) is false)
                {
                    continue;
                }

                // invariant property value on a variant content item? [#11]
                if (contentType.VariesByCulture()
                    && culture is not null
                    && propertyTypes.FirstOrDefault(p => p.Alias.InvariantEquals(fieldAlias))?.VariesByCulture() is false)
                {
                    culture = null;
                }

                var propertyValue = content.GetValue(fieldAlias, culture);
                if (propertyValue is null)
                {
                    continue;
                }

                IEnumerable<object>? parsedIndexValues = ParseIndexValues(propertyValue, fieldAlias, content);
                if (parsedIndexValues is null)
                {
                    continue;
                }

                indexValues.AddRange(parsedIndexValues);
            }

            indexFieldValues.Add(new IndexFieldValue
            {
                FieldName = filter.IndexFieldName,
                Values = indexValues.Distinct()
            });
        }

        return indexFieldValues;
    }

    private List<IndexFieldValue> SortIndexFieldValues(IContent content, string? culture)
    {
        var indexFieldValues = new List<IndexFieldValue>();
        IEnumerable<SortModel> sorts = _sortService.GetAllAsync().GetAwaiter().GetResult();
        foreach (SortModel sort in sorts)
        {
            // sorts can only have one value per content item (at least sorting only really works with one value)
            if (content.HasProperty(sort.PropertyAlias) is false)
            {
                continue;
            }

            var propertyValue = content.GetValue(sort.PropertyAlias, culture);
            if (propertyValue is null)
            {
                continue;
            }

            var indexValue = ParseIndexValues(propertyValue, sort.PropertyAlias, content)?.FirstOrDefault();
            if (indexValue is null)
            {
                continue;
            }

            indexFieldValues.Add(new IndexFieldValue
            {
                FieldName = sort.IndexFieldName,
                Values = new[] { indexValue }
            });
        }

        return indexFieldValues;
    }

    private IEnumerable<object>? ParseIndexValues(object propertyValue, string propertyAlias, IContent content)
    {
        var editorAlias = content.Properties.FirstOrDefault(p => p.Alias == propertyAlias)?.PropertyType
            .PropertyEditorAlias;
        if (editorAlias is null)
        {
            _logger.LogWarning(
                "The property type for property {propertyAlias} was not found on of content type {contentTypeAlias} - unable to index property value.",
                propertyAlias, content.ContentType.Alias);
            return null;
        }

        object[]? values = null;
        try
        {
            if (_propertyTypeParsers.TryGetValue(editorAlias, out IPropertyTypeParser? propertyTypeParser))
            {
                values = propertyTypeParser.ParseIndexFieldValue(propertyValue);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "The property type parser for property {propertyAlias} of type {editorAlias} could not parse property value {propertyValue}.",
                propertyAlias, editorAlias, propertyValue
            );
            values = null;
        }

        values ??= _fallbackPropertyTypeParser.ParseIndexFieldValue(propertyValue);
        if (values is not null)
        {
            return values;
        }

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug(
                "The property value for property {propertyAlias} of content type {contentTypeAlias} could not be indexed - it is not supported.",
                propertyAlias, content.ContentType.Alias);
        }

        return null;
    }
}
