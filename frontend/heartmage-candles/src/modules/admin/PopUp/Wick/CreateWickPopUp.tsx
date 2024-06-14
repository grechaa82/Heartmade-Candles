import { FC, useState } from 'react';

import { Wick } from '../../../../types/Wick';
import { Image } from '../../../../types/Image';
import PopUp, { PopUpProps } from '../../../../components/admin/PopUp/PopUp';
import ImageUploader from '../../../../components/admin/ImageUploader';
import ImagePreview from '../../../../components/admin/ImagePreview';
import WickForm from '../../../../components/admin/Form/Wick/WickForm';

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

  const handleOnSubmit = (data: Wick) => {
    const newWick: Wick = {
      id: 0,
      title: data.title,
      description: data.description,
      images: images,
      isActive: data.isActive,
      price: data.price,
    };
    onSave(newWick);
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
        <WickForm onSubmit={handleOnSubmit} />
      </div>
    </PopUp>
  );
};

export default CreateWickPopUp;
