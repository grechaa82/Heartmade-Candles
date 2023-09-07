import { FC, useState } from 'react';

import { PopUpProps } from './PopUp';
import ButtonWithIcon from '../../shared/ButtonWithIcon';
import IconPlusLarge from '../../../UI/IconPlusLarge';
import { Image } from '../../../types/Image';
import IconRemoveLarge from '../../../UI/IconRemoveLarge';

import Style from './AddImagesPopUp.module.css';

export interface AddImagesPopUpProps extends PopUpProps {
  onClose: () => void;
  uploadImages: (files: File[]) => Promise<string[]>;
  updateImages: (images: Image[]) => void;
}

interface ImageWithFile {
  image: Image;
  file: File;
}

const AddImagesPopUp: FC<AddImagesPopUpProps> = ({ onClose, uploadImages, updateImages }) => {
  const [images, setImages] = useState<Image[]>([]);

  const handleFileChange = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const files = event.target.files;
    if (files && files.length > 0) {
      const newFiles = Array.from(files);
      const newFileNames = await handleUpload(newFiles);
      createImage(newFiles, newFileNames);
    }
  };

  const createImage = (files: File[], fileNames: string[]) => {
    const imageWithFiles: ImageWithFile[] = files.map((file, index) => ({
      image: {
        fileName: fileNames[index],
        alternativeName: file.name,
      },
      file: file,
    }));

    setImages((prevImages) => [...prevImages, ...imageWithFiles.map((item) => item.image)]);
  };

  const handleUpload = (files: File[]): Promise<string[]> => {
    return uploadImages(files);
  };

  return (
    <div className={Style.overlay}>
      <div className={Style.popUp}>
        <button className={Style.closeButton} onClick={onClose}>
          <IconRemoveLarge color="#777" />
        </button>
        <div className={Style.container}>
          <div className={Style.divImage}>
            <input
              className={Style.inputImage}
              type="file"
              accept="*/*"
              onChange={handleFileChange}
            />
            <label htmlFor="uploadImage" className={Style.labelInput}>
              <span>Выберите файл</span> или перетяните его сюда
            </label>
          </div>
          <ButtonWithIcon
            text="Добавить фотографию"
            icon={IconPlusLarge}
            onClick={() => {
              updateImages(images);
              onClose();
            }}
            color="#2e67ea"
          />
        </div>
      </div>
    </div>
  );
};

export default AddImagesPopUp;
