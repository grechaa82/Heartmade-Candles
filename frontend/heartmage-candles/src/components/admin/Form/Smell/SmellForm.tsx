import { FC } from 'react';
import { useForm, SubmitHandler, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';

import { smellSchema, SmellType } from '../Smell/Smell.schema';
import { Smell } from '../../../../types/Smell';
import Textarea from '../../Textarea';
import CheckboxBlock from '../../CheckboxBlock';

import Style from './SmellForm.module.css';

export interface SmellFormProps {
  defaultValues?: Smell;
  onSubmit: (smell: Smell) => void;
}

const SmellForm: FC<SmellFormProps> = ({ defaultValues, onSubmit }) => {
  const {
    handleSubmit,
    formState: { isValid, errors, isDirty },
    control,
    reset,
  } = useForm({
    mode: 'onChange',
    resolver: yupResolver(smellSchema),
    defaultValues: {
      title: defaultValues ? defaultValues.title : '',
      description: defaultValues ? defaultValues.description : '',
      isActive: defaultValues ? defaultValues.isActive : false,
      price: defaultValues ? defaultValues.price : 0,
    },
  });

  const handleOnSubmit: SubmitHandler<SmellType> = (data) => {
    const newSmell: Smell = {
      id: 0,
      title: data.title,
      description: data.description,
      images: [],
      isActive: data.isActive,
      price: data.price,
    };
    onSubmit(newSmell);
    reset(newSmell);
  };

  return (
    <form
      className={`${Style.gridContainer} ${Style.formForSmell}`}
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

export default SmellForm;
