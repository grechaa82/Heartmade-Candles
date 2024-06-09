import { FC, useState } from 'react';

import { Decor } from '../../../../types/Decor';
import { Image } from '../../../../types/Image';
import PopUp, { PopUpProps } from './../PopUp';
import ImageUploader from '../../ImageUploader';
import ImagePreview from '../../ImagePreview';
import DecorForm from '../../Form/Decor/DecorForm';

import { ImagesApi } from '../../../../services/ImagesApi';

import Style from './CreateDecorPopUp.module.css';

export interface CreateDecorPopUpProps extends PopUpProps {
  title: string;
  onSave: (decor: Decor) => void;
  uploadImages?: (files: File[]) => Promise<string[]>;
}

const CreateDecorPopUp: FC<CreateDecorPopUpProps> = ({
  onClose,
  title,
  onSave,
  uploadImages,
}) => {
  const [images, setImages] = useState<Image[]>([]);

  const handleOnSubmit = (data: Decor) => {
    const decor: Decor = {
      id: data.id,
      title: data.title,
      description: data.description,
      images: images,
      isActive: data.isActive,
      price: data.price,
    };
    onSave(decor);
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
        <DecorForm onSubmit={handleOnSubmit} />
      </div>
    </PopUp>
  );
};

export default CreateDecorPopUp;
