import { FC, useState } from 'react';
import { useForm, SubmitHandler, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';

import { Candle } from '../../../../types/Candle';
import { Image } from '../../../../types/Image';
import Textarea from '../../Textarea';
import ButtonDropdown, { optionData } from '../../../shared/ButtonDropdown';
import { TypeCandle } from '../../../../types/TypeCandle';
import CheckboxBlock from '../../CheckboxBlock';
import PopUp, { PopUpProps } from '../PopUp';
import ImagePreview from '../../ImagePreview';
import ImageUploader from '../../ImageUploader';

import { ImagesApi } from '../../../../services/ImagesApi';

import Style from './CreateCandlePopUp.module.css';
import { candleSchema, CandleType } from './Candle.schema';

export interface CreateCandlePopUpProps extends PopUpProps {
  title: string;
  typeCandlesArray: TypeCandle[];
  onSave: (canlde: Candle) => void;
  uploadImages?: (files: File[]) => Promise<string[]>;
}

const CreateCandlePopUp: FC<CreateCandlePopUpProps> = ({
  onClose,
  title,
  typeCandlesArray,
  onSave,
  uploadImages,
}) => {
  const [images, setImages] = useState<Image[]>([]);

  const optionData: optionData[] = typeCandlesArray.map(({ id, title }) => ({
    id: id.toString(),
    title,
  }));

  const {
    handleSubmit,
    setValue,
    formState: { isValid, errors },
    control,
  } = useForm({
    mode: 'onChange',
    resolver: yupResolver(candleSchema),
    defaultValues: {
      title: '',
      description: '',
      isActive: false,
      price: 0,
      weightGrams: 0,
      typeCandle: typeCandlesArray[0],
    },
  });

  const onSubmit: SubmitHandler<CandleType> = (data) => {
    const candle: Candle = {
      id: 0,
      title: data.title,
      description: data.description,
      images: images,
      isActive: data.isActive,
      price: data.price,
      weightGrams: data.weightGrams,
      typeCandle: data.typeCandle,
      createdAt: new Date().toISOString(),
    };
    onSave(candle);
    onClose();
  };

  const handleOnClose = async () => {
    await deleteImages(images.map((image) => image.fileName));
    onClose();
  };

  const deleteImages = async (uploadedImages?: string[]) => {
    if (uploadedImages) {
      const imagesToDelete = uploadedImages;
      await ImagesApi.deleteImages(imagesToDelete);
    }
  };

  const processUpload = async (files: File[]) => {
    const newFileNames = await uploadImages(files);
    const newImages = files.map((file, index) => ({
      fileName: newFileNames[index],
      alternativeName: file.name,
    }));
    setImages((prevImages) => [...prevImages, ...newImages]);
  };

  console.log(
    'fass',
    errors.title ||
      errors.description ||
      errors.price ||
      errors.isActive ||
      errors.weightGrams ||
      errors.typeCandle
      ? Style.invalid
      : isValid
      ? Style.valid
      : Style.default,
  );

  return (
    <PopUp onClose={handleOnClose}>
      <div className={Style.container}>
        <p className={Style.title}>{title}</p>
        <div className={Style.imageBlock}>
          <ImageUploader uploadImages={processUpload} />
          <ImagePreview images={images} />
        </div>
        <form
          className={`${Style.gridContainer} ${Style.formForCandle}`}
          onSubmit={handleSubmit(onSubmit)}
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
          <div className={`${Style.formItem} ${Style.itemWeightGrams}`}>
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
              <p className={Style.validationError}>
                {errors.weightGrams.message}
              </p>
            )}
          </div>
          <div className={`${Style.formItem} ${Style.itemType}`}>
            <Controller
              name="typeCandle"
              control={control}
              render={({ field }) => (
                <ButtonDropdown
                  text="Тип свечи"
                  selected={{
                    id: field.value.id.toString(),
                    title: field.value.title,
                    // id: typeCandlesArray[0].id.toString(),
                    // title: typeCandlesArray[0].title,
                  }}
                  options={optionData}
                  onChange={(value) =>
                    setValue(
                      'typeCandle',
                      {
                        id: parseInt(value.id),
                        title: value.title,
                      },
                      { shouldTouch: true },
                    )
                  }
                />
              )}
            />
            {errors?.weightGrams && (
              <p className={Style.validationError}>
                {errors.weightGrams.message}
              </p>
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
                />
              )}
            />
            {errors?.description && (
              <p className={Style.validationError}>
                {errors.description.message}
              </p>
            )}
          </div>
          {onSave && (
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
      </div>
    </PopUp>
  );
};

export default CreateCandlePopUp;
