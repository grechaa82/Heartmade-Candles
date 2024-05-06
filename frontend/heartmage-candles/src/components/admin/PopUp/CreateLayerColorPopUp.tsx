import { FC, useState, ChangeEvent } from 'react';

import { LayerColor } from '../../../types/LayerColor';
import Textarea from '../Textarea';
import CheckboxBlock from '../CheckboxBlock';
import PopUp, { PopUpProps } from './PopUp';
import ImageUploader from '../ImageUploader';
import ImagePreview from '../ImagePreview';

import { ImagesApi } from '../../../services/ImagesApi';

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
  const [layerColor, setLayerColor] = useState<LayerColor>({
    id: 0,
    title: '',
    description: '',
    images: [],
    isActive: false,
    pricePerGram: 0,
  });
  const [isModified, setIsModified] = useState(false);

  const handleChangeTitle = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setLayerColor((prev) => ({ ...prev, title: event.target.value }));
    setIsModified(true);
  };

  const handleChangePricePerGram = (
    event: ChangeEvent<HTMLTextAreaElement>,
  ) => {
    setLayerColor((prev) => ({
      ...prev,
      pricePerGram: parseFloat(event.target.value),
    }));
    setIsModified(true);
  };

  const handleChangeIsActive = (isActive: boolean) => {
    setLayerColor((prev) => ({ ...prev, isActive }));
    setIsModified(true);
  };

  const handleChangeDescription = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setLayerColor((prev) => ({ ...prev, description: event.target.value }));
    setIsModified(true);
  };

  const handleOnClose = async () => {
    await deleteImages(layerColor.images.map((image) => image.fileName));
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
    setLayerColor((prev) => ({
      ...prev,
      images: [...(prev.images || []), ...newImages],
    }));
  };

  return (
    <PopUp onClose={handleOnClose}>
      <div className={Style.container}>
        <p className={Style.title}>{title}</p>
        <div className={Style.imageBlock}>
          <ImageUploader uploadImages={processUpload} />
          <ImagePreview images={layerColor.images} />
        </div>
        <form className={`${Style.gridContainer} ${Style.formForLayerColor}`}>
          <div className={`${Style.formItem} ${Style.itemTitle}`}>
            <Textarea
              text={layerColor.title}
              label="Название"
              limitation={{ limit: 48 }}
              onChange={handleChangeTitle}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemPrice}`}>
            <Textarea
              text={layerColor.pricePerGram.toString()}
              label="Стоимость за грамм"
              onChange={handleChangePricePerGram}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemActive}`}>
            <CheckboxBlock
              text="Активна"
              checked={layerColor.isActive}
              onChange={handleChangeIsActive}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemDescription}`}>
            <Textarea
              text={layerColor.description}
              label="Описание"
              height={175}
              limitation={{ limit: 256 }}
              onChange={handleChangeDescription}
            />
          </div>
          {onSave && (
            <button
              type="button"
              className={`${Style.saveButton} ${
                isModified && Style.activeSaveButton
              }`}
              onClick={() => {
                onSave(layerColor);
                onClose();
              }}
            >
              Сохранить
            </button>
          )}
        </form>
      </div>
    </PopUp>
  );
};

export default CreateLayerColorPopUp;
