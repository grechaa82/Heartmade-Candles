import * as yup from 'yup';
import { TypeCandle } from '../../../../types/TypeCandle';

const MaxTitleLength = 48;
const MaxDescriptionLength = 256;

export const candleSchema = yup
  .object()
  .shape({
    title: yup
      .string()
      .required('Title is required')
      .max(MaxTitleLength, `Maximum length is ${MaxTitleLength}`),
    description: yup
      .string()
      .required('Description is required')
      .max(MaxDescriptionLength, `Maximum length is ${MaxDescriptionLength}`),
    isActive: yup.boolean().required(),
    price: yup
      .number()
      .required()
      .moreThan(0, 'Cannot be 0 or less')
      .typeError('There must be number'),
    weightGrams: yup
      .number()
      .required()
      .moreThan(0, 'Cannot be 0 or less')
      .typeError('There must be number'),
    typeCandle: yup
      .object()
      .shape({
        id: yup.number().required(),
        title: yup.string().required(),
      })
      .required(),
  })
  .required();

export type CandleType = {
  title: string;
  description: string;
  isActive: boolean;
  price: number;
  weightGrams: number;
  typeCandle: TypeCandle;
};
