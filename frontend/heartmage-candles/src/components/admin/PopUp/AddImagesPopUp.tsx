import { FC, useState } from 'react';

import PopUp, { PopUpProps } from '../../shared/PopUp/PopUp';
import ButtonWithIcon from '../../shared/ButtonWithIcon';
import IconPlusLarge from '../../../UI/IconPlusLarge';
import { Image } from '../../../types/Image';
import Picture from '../../shared/Picture';

import Style from './AddImagesPopUp.module.css';

export interface AddImagesPopUpProps extends PopUpProps {
  onClose: (uploadedImages?: string[]) => void;
  uploadImages: (files: File[]) => Promise<string[]>;
  updateImages: (images: Image[]) => void;
}

interface ImageWithFile {
  image: Image;
  file: File;
}

const AddImagesPopUp: FC<AddImagesPopUpProps> = ({
  onClose,
  uploadImages,
  updateImages,
}) => {
  const [images, setImages] = useState<Image[]>();
  const [dragging, setDragging] = useState(false);

  const handleFileChange = async (files: File[]) => {
    if (files && files.length > 0) {
      const newFileNames = await uploadImages(files);
      createImage(files, newFileNames);
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
    const newImages: Image[] = imageWithFiles.map((item) => item.image);
    setImages((prev) => (prev ? [...prev, ...newImages] : newImages));
  };

  const handleDragOver = (e: React.DragEvent<HTMLDivElement>) => {
    e.preventDefault();
    setDragging(true);
  };

  const handleDragLeave = () => {
    setDragging(false);
  };

  const handleDrop = (e: React.DragEvent<HTMLDivElement>) => {
    e.preventDefault();
    setDragging(false);

    const filesArray = Array.from(e.dataTransfer.files);
    const imageFiles = filesArray.filter((file) =>
      file.type.startsWith('image/'),
    );

    if (imageFiles.length > 0) {
      handleFileChange(imageFiles);
    }
  };

  const handleFileInput = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files.length) {
      const filesArray = Array.from(e.target.files);
      handleFileChange(filesArray);
    }
  };

  const handleOnClose = () => {
    onClose(images && images.map((item) => item.fileName));
  };

  return (
    <PopUp onClose={handleOnClose}>
      <div className={Style.container}>
        <div
          className={`${Style.divImage} ${dragging ? Style.dragging : ''}`}
          onDragOver={handleDragOver}
          onDragLeave={handleDragLeave}
          onDrop={handleDrop}
        >
          <label className={Style.labelInput} htmlFor="fileInput">
            <span>Выберите файл</span> <br /> или перетащите его сюда
          </label>
          <input
            className={Style.input}
            id="fileInput"
            type="file"
            accept="image/*"
            multiple
            onChange={handleFileInput}
          />
        </div>
        {images && (
          <div className={Style.imagePrevBlock}>
            {images.map((image, index) => (
              <div className={Style.imagePrev} key={index}>
                <Picture
                  name={image.fileName}
                  alt={image.alternativeName}
                  className={Style.squareImage}
                  sourceSettings={[
                    {
                      size: 'small',
                    },
                  ]}
                />
              </div>
            ))}
          </div>
        )}
        <ButtonWithIcon
          text="Добавить фотографию"
          icon={IconPlusLarge}
          onClick={() => {
            updateImages(images ? images : []);
            onClose();
          }}
          color="#2e67ea"
        />
      </div>
    </PopUp>
  );
};

export default AddImagesPopUp;
