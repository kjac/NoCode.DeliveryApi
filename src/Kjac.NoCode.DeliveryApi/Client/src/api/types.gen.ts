// This file is auto-generated by @hey-api/openapi-ts

export type AddClientRequestModel = {
    name: string;
    origin: string;
    previewUrlPath?: (string) | null;
    publishedUrlPath?: (string) | null;
    culture?: (string) | null;
};

export type AddFilterRequestModel = {
    name: string;
    propertyAliases: Array<(string)>;
    filterMatchType: FilterMatchTypeModel;
    primitiveFieldType: PrimitiveFieldTypeModel;
};

export type AddSortRequestModel = {
    name: string;
    propertyAlias: string;
    primitiveFieldType: PrimitiveFieldTypeModel;
};

export type ClientModel = {
    id: string;
    name: string;
    origin: string;
    previewUrlPath?: (string) | null;
    publishedUrlPath?: (string) | null;
    culture?: (string) | null;
};

export type EventMessageTypeModel = 'Default' | 'Info' | 'Error' | 'Success' | 'Warning';

export type FilterListModel = {
    filters: Array<(FilterModel)>;
    canAddFilter: boolean;
};

export type FilterMatchTypeModel = 'Exact' | 'Partial';

export type FilterModel = {
    id: string;
    name: string;
    alias: string;
    fieldName: string;
    primitiveFieldType: PrimitiveFieldTypeModel;
    propertyAliases: Array<(string)>;
    filterMatchType: FilterMatchTypeModel;
};

export type NotificationHeaderModel = {
    message: string;
    category: string;
    type: EventMessageTypeModel;
};

export type PrimitiveFieldTypeModel = 'String' | 'Number' | 'Date';

export type ProblemDetails = {
    type?: (string) | null;
    title?: (string) | null;
    status?: (number) | null;
    detail?: (string) | null;
    instance?: (string) | null;
    [key: string]: (unknown | string | number) | undefined;
};

export type SortListModel = {
    sorts: Array<(SortModel)>;
    canAddSort: boolean;
};

export type SortModel = {
    id: string;
    name: string;
    alias: string;
    fieldName: string;
    primitiveFieldType: PrimitiveFieldTypeModel;
    propertyAlias: string;
};

export type UpdateClientRequestModel = {
    name: string;
    origin: string;
    previewUrlPath?: (string) | null;
    publishedUrlPath?: (string) | null;
    culture?: (string) | null;
};

export type UpdateFilterRequestModel = {
    name: string;
    propertyAliases: Array<(string)>;
};

export type UpdateSortRequestModel = {
    name: string;
    propertyAlias: string;
};

export type PostNoCodeDeliveryApiClientData = {
    requestBody?: (AddClientRequestModel);
};

export type PostNoCodeDeliveryApiClientResponse = (string);

export type GetNoCodeDeliveryApiClientResponse = (Array<(ClientModel)>);

export type DeleteNoCodeDeliveryApiClientByIdData = {
    id: string;
};

export type DeleteNoCodeDeliveryApiClientByIdResponse = (string);

export type PutNoCodeDeliveryApiClientByIdData = {
    id: string;
    requestBody?: (UpdateClientRequestModel);
};

export type PutNoCodeDeliveryApiClientByIdResponse = (string);

export type PostNoCodeDeliveryApiFilterData = {
    requestBody?: (AddFilterRequestModel);
};

export type PostNoCodeDeliveryApiFilterResponse = (string);

export type GetNoCodeDeliveryApiFilterResponse = ((FilterListModel));

export type DeleteNoCodeDeliveryApiFilterByIdData = {
    id: string;
};

export type DeleteNoCodeDeliveryApiFilterByIdResponse = (string);

export type PutNoCodeDeliveryApiFilterByIdData = {
    id: string;
    requestBody?: (UpdateFilterRequestModel);
};

export type PutNoCodeDeliveryApiFilterByIdResponse = (string);

export type PostNoCodeDeliveryApiSortData = {
    requestBody?: (AddSortRequestModel);
};

export type PostNoCodeDeliveryApiSortResponse = (string);

export type GetNoCodeDeliveryApiSortResponse = ((SortListModel));

export type DeleteNoCodeDeliveryApiSortByIdData = {
    id: string;
};

export type DeleteNoCodeDeliveryApiSortByIdResponse = (string);

export type PutNoCodeDeliveryApiSortByIdData = {
    id: string;
    requestBody?: (UpdateSortRequestModel);
};

export type PutNoCodeDeliveryApiSortByIdResponse = (string);