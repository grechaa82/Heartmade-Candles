import * as yup from 'yup';

const MaxTitleLength = 48;
const MaxDescriptionLength = 256;

export const layerColorSchema = yup
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
    pricePerGram: yup
      .number()
      .required()
      .moreThan(0, 'Cannot be 0 or less')
      .typeError('There must be number'),
  })
  .required();

export type LayerColorType = {
  title: string;
  description: string;
  isActive: boolean;
  pricePerGram: number;
};
