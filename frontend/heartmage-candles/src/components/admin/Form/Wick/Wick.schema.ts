import * as yup from 'yup';

const MaxTitleLength = 48;
const MaxDescriptionLength = 256;

export const wickSchema = yup
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
  })
  .required();

export type WickType = {
  title: string;
  description: string;
  isActive: boolean;
  price: number;
};
