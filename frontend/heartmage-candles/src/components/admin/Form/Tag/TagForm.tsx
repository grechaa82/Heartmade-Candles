import { FC } from 'react';
import { useForm, SubmitHandler, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';

import { tagSchema, TagType } from './Tag.schema';
import { TagData } from '../../../shared/Tag';
import Textarea from '../../Textarea';

import Style from './TagForm.module.css';

export interface TagFormProps {
  defaultValues?: TagData;
  onSubmit: (tag: TagData) => void;
}

const TagForm: FC<TagFormProps> = ({ defaultValues, onSubmit }) => {
  const {
    handleSubmit,
    formState: { isValid, errors, isDirty },
    control,
    reset,
  } = useForm({
    mode: 'onChange',
    resolver: yupResolver(tagSchema),
    defaultValues: {
      text: defaultValues ? defaultValues.text : '',
    },
  });

  const handleOnSubmit: SubmitHandler<TagType> = (data) => {
    const newTag: TagData = {
      id: 0,
      text: data.text,
    };
    onSubmit(newTag);
    reset(newTag);
  };

  return (
    <form
      className={`${Style.gridContainer} ${Style.formForDecor}`}
      onSubmit={handleSubmit(handleOnSubmit)}
    >
      <div className={`${Style.formItem} ${Style.itemTitle}`}>
        <Controller
          name="text"
          control={control}
          render={({ field }) => (
            <Textarea
              text={field.value}
              label="Название"
              onChange={field.onChange}
            />
          )}
        />
        {errors?.text && (
          <p className={Style.validationError}>{errors.text.message}</p>
        )}
      </div>
      {(!defaultValues || isDirty) && (
        <button
          type="submit"
          className={`${Style.saveButton} ${
            errors.text ? Style.invalid : isValid ? Style.valid : Style.default
          }`}
          disabled={!isValid || Object.keys(errors).length > 0}
        >
          Сохранить
        </button>
      )}
    </form>
  );
};

export default TagForm;
