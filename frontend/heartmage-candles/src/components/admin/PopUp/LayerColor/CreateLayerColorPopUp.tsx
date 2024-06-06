import { FC, useState } from 'react';
import { useForm, SubmitHandler, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';

import { LayerColor } from '../../../../types/LayerColor';
import { Image } from '../../../../types/Image';
import Textarea from '../../Textarea';
import CheckboxBlock from '../../CheckboxBlock';
import PopUp, { PopUpProps } from '../PopUp';
import ImageUploader from '../../ImageUploader';
import ImagePreview from '../../ImagePreview';
import { layerColorSchema, LayerColorType } from './LayerColor.schema';

import { ImagesApi } from '../../../../services/ImagesApi';

import Style from './CreateLayerColorPopUp.module.css';

export interface CreateLayerColorPopUpProps extends PopUpProps {
  title: string;
  onSave: (layerColor: LayerColor) => void;
  uploadImages?: (files: File[]) => Promise<string[]>;
}

const CreateLayerColorPopUp: FC<CreateLayerColorPopUpProps> = ({
  onClose,
  title,
  onSave,
  uploadImages,
}) => {
  const [images, setImages] = useState<Image[]>([]);

  const {
    handleSubmit,
    formState: { isValid, errors },
    control,
  } = useForm({
    mode: 'onChange',
    resolver: yupResolver(layerColorSchema),
    defaultValues: {
      title: '',
      description: '',
      isActive: false,
      pricePerGram: 0,
    },
  });

  const onSubmit: SubmitHandler<LayerColorType> = (data) => {
    const layerColor: LayerColor = {
      id: 0,
      title: data.title,
      description: data.description,
      images: images,
      isActive: data.isActive,
      pricePerGram: data.pricePerGram,
    };
    onSave(layerColor);
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

  return (
    <PopUp onClose={handleOnClose}>
      <div className={Style.container}>
        <p className={Style.title}>{title}</p>
        <div className={Style.imageBlock}>
          <ImageUploader uploadImages={processUpload} />
          <ImagePreview images={images} />
        </div>
        <form
          className={`${Style.gridContainer} ${Style.formForLayerColor}`}
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
              <p className={Style.validationError}>
                {errors.pricePerGram.message}
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
        </form>
      </div>
    </PopUp>
  );
};

export default CreateLayerColorPopUp;
