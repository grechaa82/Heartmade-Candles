import * as yup from 'yup';

const MaxTextLength = 48;

export const tagSchema = yup
  .object()
  .shape({
    text: yup
      .string()
      .required('Title is required')
      .max(MaxTextLength, `Maximum length is ${MaxTextLength}`),
  })
  .required();

export type TagType = {
  text: string;
};
