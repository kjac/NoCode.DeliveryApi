﻿using System.Runtime.Serialization;

namespace Kjac.NoCode.DeliveryApi.ViewModels;

[DataContract]
internal abstract class ViewModelBase
{
    [DataMember(Name = "key")]
    public required Guid Key { get; init; }

    [DataMember(Name = "name")]
    public required string Name { get; init; }
}
