import { FC, useState } from 'react';

import { Candle } from '../../../../types/Candle';
import { Image } from '../../../../types/Image';
import { TypeCandle } from '../../../../types/TypeCandle';
import PopUp, { PopUpProps } from '../../../../components/shared/PopUp/PopUp';
import ImagePreview from '../../../../components/admin/ImagePreview';
import ImageUploader from '../../../../components/admin/ImageUploader';
import CandleForm from '../../../../components/admin/Form/Candle/CandleForm';

import { ImagesApi } from '../../../../services/ImagesApi';

import Style from './CreateCandlePopUp.module.css';

export interface CreateCandlePopUpProps extends PopUpProps {
  title: string;
  typeCandlesArray: TypeCandle[];
  onSave: (candle: Candle) => void;
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

  const handleOnSubmit = (data: Candle) => {
    const candle: Candle = {
      id: data.id,
      title: data.title,
      description: data.description,
      images: images,
      isActive: data.isActive,
      price: data.price,
      weightGrams: data.weightGrams,
      typeCandle: data.typeCandle,
      createdAt: data.createdAt,
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

  return (
    <PopUp onClose={handleOnClose}>
      <div className={Style.container}>
        <p className={Style.title}>{title}</p>
        <div className={Style.imageBlock}>
          <ImageUploader uploadImages={processUpload} />
          <ImagePreview images={images} />
        </div>
        <CandleForm
          typeCandlesArray={typeCandlesArray}
          onSubmit={handleOnSubmit}
        />
      </div>
    </PopUp>
  );
};

export default CreateCandlePopUp;
