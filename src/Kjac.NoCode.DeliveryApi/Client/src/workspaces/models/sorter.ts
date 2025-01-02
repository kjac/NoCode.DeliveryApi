import {PrimitiveFieldTypeModel} from '../../api';

export interface SorterDetails extends SorterBase {
  id?: string;
  alias: string;
  fieldName: string;
}

export interface SorterBase {
  name: string;
  propertyAlias: string;
  fieldType: PrimitiveFieldTypeModel;
}
