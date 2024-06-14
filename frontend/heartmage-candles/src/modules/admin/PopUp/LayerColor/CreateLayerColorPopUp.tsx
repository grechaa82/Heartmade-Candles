import { FC, useState } from 'react';

import { LayerColor } from '../../../../types/LayerColor';
import { Image } from '../../../../types/Image';
import PopUp, { PopUpProps } from '../../../../components/admin/PopUp/PopUp';
import ImageUploader from '../../../../components/admin/ImageUploader';
import ImagePreview from '../../../../components/admin/ImagePreview';
import LayerColorForm from '../../../../components/admin/Form/LayerColor/LayerColorForm';

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

  const handleOnSubmit = (data: LayerColor) => {
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
        <LayerColorForm onSubmit={handleOnSubmit} />
      </div>
    </PopUp>
  );
};

export default CreateLayerColorPopUp;
