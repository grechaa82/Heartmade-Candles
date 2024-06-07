import { FC } from 'react';
import { useForm, SubmitHandler, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';

import { candleSchema, CandleType } from './Candle.schema';
import { TypeCandle } from '../../../../types/TypeCandle';
import ButtonDropdown, { optionData } from '../../../shared/ButtonDropdown';
import { Candle } from '../../../../types/Candle';
import Textarea from '../../Textarea';
import CheckboxBlock from '../../CheckboxBlock';

import Style from './CandleForm.module.css';

export interface CandleFormProps {
  typeCandlesArray: TypeCandle[];
  defaultValues?: Candle;
  onSubmit: (canlde: Candle) => void;
}

const CandleForm: FC<CandleFormProps> = ({
  typeCandlesArray,
  defaultValues,
  onSubmit,
}) => {
  const optionData: optionData[] = typeCandlesArray.map(({ id, title }) => ({
    id: id.toString(),
    title,
  }));

  const {
    handleSubmit,
    setValue,
    formState: { isValid, errors, isDirty },
    control,
  } = useForm({
    mode: 'onChange',
    resolver: yupResolver(candleSchema),
    defaultValues: {
      title: defaultValues ? defaultValues.title : '',
      description: defaultValues ? defaultValues.description : '',
      isActive: defaultValues ? defaultValues.isActive : false,
      price: defaultValues ? defaultValues.price : 0,
      weightGrams: defaultValues ? defaultValues.weightGrams : 0,
      typeCandle: defaultValues
        ? defaultValues.typeCandle
        : typeCandlesArray[0],
    },
  });

  const handleOnSubmit: SubmitHandler<CandleType> = (data) => {
    const candle: Candle = {
      id: 0,
      title: data.title,
      description: data.description,
      images: [],
      isActive: data.isActive,
      price: data.price,
      weightGrams: data.weightGrams,
      typeCandle: data.typeCandle,
      createdAt: new Date().toISOString(),
    };
    onSubmit(candle);
  };

  return (
    <form
      className={Style.gridContainer}
      onSubmit={handleSubmit(handleOnSubmit)}
    >
      <div className={Style.itemTitle}>
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
      <div className={Style.itemPrice}>
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
      <div className={Style.itemWeightGrams}>
        <Controller
          name="weightGrams"
          control={control}
          render={({ field }) => (
            <Textarea
              text={field.value.toString()}
              label="Вес в граммах"
              onChange={field.onChange}
            />
          )}
        />
        {errors?.weightGrams && (
          <p className={Style.validationError}>{errors.weightGrams.message}</p>
        )}
      </div>
      <div className={Style.itemType}>
        <Controller
          name="typeCandle"
          control={control}
          render={({ field }) => (
            <ButtonDropdown
              text="Тип свечи"
              selected={{
                id: field.value.id.toString(),
                title: field.value.title,
              }}
              options={optionData}
              onChange={(value) =>
                setValue(
                  'typeCandle',
                  {
                    id: parseInt(value.id),
                    title: value.title,
                  },
                  { shouldTouch: true, shouldDirty: true },
                )
              }
            />
          )}
        />
        {errors?.typeCandle && (
          <p className={Style.validationError}>{errors.typeCandle.message}</p>
        )}
      </div>
      <div className={Style.itemActive}>
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
      <div className={Style.itemDescription}>
        <Controller
          name="description"
          control={control}
          render={({ field }) => (
            <Textarea
              text={field.value}
              label="Описание"
              onChange={field.onChange}
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
            errors.isActive ||
            errors.weightGrams ||
            errors.typeCandle
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

export default CandleForm;
