import { FC } from 'react';
import { useForm, SubmitHandler, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';

import { layerColorSchema, LayerColorType } from './LayerColor.schema';
import { LayerColor } from '../../../../types/LayerColor';
import Textarea from '../../Textarea';
import CheckboxBlock from '../../CheckboxBlock';

import Style from './LayerColorForm.module.css';

export interface LayerColorFormProps {
  defaultValues?: LayerColor;
  onSubmit: (layerColor: LayerColor) => void;
}

const LayerColorForm: FC<LayerColorFormProps> = ({
  defaultValues,
  onSubmit,
}) => {
  const {
    handleSubmit,
    formState: { isValid, errors, isDirty },
    control,
    reset,
  } = useForm({
    mode: 'onChange',
    resolver: yupResolver(layerColorSchema),
    defaultValues: {
      title: defaultValues ? defaultValues.title : '',
      description: defaultValues ? defaultValues.description : '',
      isActive: defaultValues ? defaultValues.isActive : false,
      pricePerGram: defaultValues ? defaultValues.pricePerGram : 0,
    },
  });

  const handleOnSubmit: SubmitHandler<LayerColorType> = (data) => {
    const layerColor: LayerColor = {
      id: 0,
      title: data.title,
      description: data.description,
      images: [],
      isActive: data.isActive,
      pricePerGram: data.pricePerGram,
    };
    onSubmit(layerColor);
    reset(layerColor);
  };

  return (
    <form
      className={`${Style.gridContainer} ${Style.formForLayerColor}`}
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
          name="pricePerGram"
          control={control}
          render={({ field }) => (
            <Textarea
              text={field.value.toString()}
              label="Стоимость за грамм"
              onChange={field.onChange}
            />
          )}
        />
        {errors?.pricePerGram && (
          <p className={Style.validationError}>{errors.pricePerGram.message}</p>
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
            errors.pricePerGram ||
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

export default LayerColorForm;
