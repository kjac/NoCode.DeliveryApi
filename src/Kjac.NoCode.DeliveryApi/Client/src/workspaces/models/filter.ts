import {FilterMatchTypeModel, PrimitiveFieldTypeModel} from '../../api';

export interface FilterDetails extends FilterBase {
  id?: string;
  alias: string;
  fieldName: string;
}

export interface FilterBase {
  name: string;
  propertyAliases: Array<string>;
  matchType: FilterMatchTypeModel;
  fieldType: PrimitiveFieldTypeModel;
}
