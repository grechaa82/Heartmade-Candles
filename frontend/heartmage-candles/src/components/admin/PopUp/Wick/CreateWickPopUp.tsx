import { FC, useState } from 'react';
import { useForm, SubmitHandler, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';

import { Wick } from '../../../../types/Wick';
import { Image } from '../../../../types/Image';
import Textarea from '../../Textarea';
import CheckboxBlock from '../../CheckboxBlock';
import PopUp, { PopUpProps } from '../PopUp';
import ImageUploader from '../../ImageUploader';
import ImagePreview from '../../ImagePreview';
import { wickSchema, WickType } from './Wick.schema';

import { ImagesApi } from '../../../../services/ImagesApi';

import Style from './CreateWickPopUp.module.css';

export interface CreateWickPopUpProps extends PopUpProps {
  title: string;
  onSave: (wick: Wick) => void;
  uploadImages?: (files: File[]) => Promise<string[]>;
}

const CreateWickPopUp: FC<CreateWickPopUpProps> = ({
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
    resolver: yupResolver(wickSchema),
    defaultValues: {
      title: '',
      description: '',
      isActive: false,
      price: 0,
    },
  });

  const onSubmit: SubmitHandler<WickType> = (data) => {
    const wick: Wick = {
      id: 0,
      title: data.title,
      description: data.description,
      images: images,
      isActive: data.isActive,
      price: data.price,
    };
    onSave(wick);
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
          className={`${Style.gridContainer} ${Style.formForSmell}`}
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
        </form>
      </div>
    </PopUp>
  );
};

export default CreateWickPopUp;
