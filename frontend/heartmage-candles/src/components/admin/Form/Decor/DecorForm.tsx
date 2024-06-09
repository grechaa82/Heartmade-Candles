import { FC } from 'react';
import { useForm, SubmitHandler, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';

import { decorSchema, DecorType } from './Decor.schema';
import { Decor } from '../../../../types/Decor';
import Textarea from '../../Textarea';
import CheckboxBlock from '../../CheckboxBlock';

import Style from './DecorForm.module.css';

export interface DecorFormProps {
  defaultValues?: Decor;
  onSubmit: (decor: Decor) => void;
}

const DecorForm: FC<DecorFormProps> = ({ defaultValues, onSubmit }) => {
  const {
    handleSubmit,
    formState: { isValid, errors, isDirty },
    control,
    reset,
  } = useForm({
    mode: 'onChange',
    resolver: yupResolver(decorSchema),
    defaultValues: {
      title: defaultValues ? defaultValues.title : '',
      description: defaultValues ? defaultValues.description : '',
      isActive: defaultValues ? defaultValues.isActive : false,
      price: defaultValues ? defaultValues.price : 0,
    },
  });

  const handleOnSubmit: SubmitHandler<DecorType> = (data) => {
    const decor: Decor = {
      id: 0,
      title: data.title,
      description: data.description,
      images: [],
      isActive: data.isActive,
      price: data.price,
    };
    onSubmit(decor);
    reset(decor);
  };

  return (
    <form
      className={`${Style.gridContainer} ${Style.formForDecor}`}
      onSubmit={handleSubmit(handleOnSubmit)}
    >
      <div className={`${Style.formItem} ${Style.itemTitle}`}>
        <Controller
          name="title"
          control={control}
          render={({ field }) => (
            <Textarea
              text={field.value}
              label="Название"
              onChange={field.onChange}
            />
          )}
        />
        {errors?.title && (
          <p className={Style.validationError}>{errors.title.message}</p>
        )}
      </div>
      <div className={`${Style.formItem} ${Style.itemPrice}`}>
        <Controller
          name="price"
          control={control}
          render={({ field }) => (
            <Textarea
              text={field.value.toString()}
              label="Стоимость"
              onChange={field.onChange}
            />
          )}
        />
        {errors?.price && (
          <p className={Style.validationError}>{errors.price.message}</p>
        )}
      </div>
      <div className={`${Style.formItem} ${Style.itemActive}`}>
        <Controller
          name="isActive"
          control={control}
          render={({ field }) => (
            <CheckboxBlock
              text="Активна"
              checked={field.value}
              onChange={field.onChange}
            />
          )}
        />
        {errors?.isActive && (
          <p className={Style.validationError}>{errors.isActive.message}</p>
        )}
      </div>
      <div className={`${Style.formItem} ${Style.itemDescription}`}>
        <Controller
          name="description"
          control={control}
          render={({ field }) => (
            <Textarea
              text={field.value}
              label="Описание"
              onChange={field.onChange}
              height={120}
            />
          )}
        />
        {errors?.description && (
          <p className={Style.validationError}>{errors.description.message}</p>
        )}
      </div>
      {(!defaultValues || isDirty) && (
        <button
          type="submit"
          className={`${Style.saveButton} ${
            errors.title ||
            errors.description ||
            errors.price ||
            errors.isActive
              ? Style.invalid
              : isValid
              ? Style.valid
              : Style.default
          }`}
          disabled={!isValid || Object.keys(errors).length > 0}
        >
          Сохранить
        </button>
      )}
    </form>
  );
};

export default DecorForm;
