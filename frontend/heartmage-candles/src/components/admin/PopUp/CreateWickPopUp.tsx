import { FC, useState, ChangeEvent } from 'react';

import { Wick } from '../../../types/Wick';
import Textarea from '../Textarea';
import CheckboxBlock from '../CheckboxBlock';
import PopUp, { PopUpProps } from './PopUp';
import ImageUploader from '../ImageUploader';
import ImagePreview from '../ImagePreview';

import { ImagesApi } from '../../../services/ImagesApi';

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
  const [wick, setWick] = useState<Wick>({
    id: 0,
    title: '',
    description: '',
    images: [],
    isActive: false,
    price: 0,
  });
  const [isModified, setIsModified] = useState(false);

  const handleChangeTitle = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setWick((prev) => ({ ...prev, title: event.target.value }));
    setIsModified(true);
  };

  const handleChangePrice = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setWick((prev) => ({
      ...prev,
      price: parseFloat(event.target.value),
    }));
    setIsModified(true);
  };

  const handleChangeIsActive = (isActive: boolean) => {
    setWick((prev) => ({ ...prev, isActive }));
    setIsModified(true);
  };

  const handleChangeDescription = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setWick((prev) => ({ ...prev, description: event.target.value }));
    setIsModified(true);
  };

  const handleOnClose = async () => {
    await deleteImages(wick.images.map((image) => image.fileName));
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
    setWick((prev) => ({
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
          <ImagePreview images={wick.images} />
        </div>
        <form className={`${Style.gridContainer} ${Style.formForWick}`}>
          <div className={`${Style.formItem} ${Style.itemTitle}`}>
            <Textarea
              text={wick.title}
              label="Название"
              limitation={{ limit: 48 }}
              onChange={handleChangeTitle}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemPrice}`}>
            <Textarea
              text={wick.price.toString()}
              label="Стоимость"
              onChange={handleChangePrice}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemActive}`}>
            <CheckboxBlock
              text="Активна"
              checked={wick.isActive}
              onChange={handleChangeIsActive}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemDescription}`}>
            <Textarea
              text={wick.description}
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
                onSave(wick);
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

export default CreateWickPopUp;
