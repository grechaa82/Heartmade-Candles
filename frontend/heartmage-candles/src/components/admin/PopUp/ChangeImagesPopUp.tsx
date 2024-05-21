import { FC, useState, DragEvent } from 'react';

import PopUp, { PopUpProps } from './PopUp';
import Button from '../../shared/Button';
import { Image } from '../../../types/Image';
import IconTrashLarge from '../../../UI/IconTrashLarge';
import Picture from '../../shared/Picture';

import Style from './ChangeImagesPopUp.module.css';

export interface ChangeImagesPopUpProps extends PopUpProps {
  images: Image[];
  onClose: () => void;
  updateImages: (images: Image[]) => void;
  onRemove: (image: Image) => void;
}

const ChangeImagesPopUp: FC<ChangeImagesPopUpProps> = ({
  images,
  onClose,
  updateImages,
  onRemove,
}) => {
  const [newImages, setNewImages] = useState<Image[]>(images);
  const [currentImage, setCurrentImage] = useState<Image>();

  function handleDragStart(e: DragEvent<HTMLDivElement>, image: Image) {
    setCurrentImage(image);
  }
  function handleDragOver(e: DragEvent<HTMLDivElement>) {
    e.preventDefault();
  }
  function handleDrop(e: DragEvent<HTMLDivElement>, image: Image) {
    e.preventDefault();
    const newStateImages: Image[] = newImages.map((i) => {
      if (i.fileName === image.fileName && currentImage) {
        return {
          ...i,
          fileName: currentImage.fileName,
          alternativeName: currentImage.alternativeName,
        };
      }
      if (i.fileName === currentImage?.fileName) {
        return {
          ...i,
          fileName: image?.fileName,
          alternativeName: image?.alternativeName,
        };
      }
      return i;
    });
    setNewImages(newStateImages);
  }

  return (
    <PopUp onClose={onClose}>
      <div className={Style.container}>
        {newImages.map((image, index) => (
          <div
            className={Style.imageBlock}
            onDragStart={(e) => handleDragStart(e, image)}
            onDragOver={(e) => handleDragOver(e)}
            onDrop={(e) => handleDrop(e, image)}
            draggable={true}
            key={index}
          >
            <div className={Style.imagePrev}>
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
            <div className={Style.info}>
              <p className={Style.title}>{image.fileName}</p>
              <p className={Style.description}>{image.alternativeName}</p>
            </div>
            <button
              onClick={() => {
                onRemove(image);
                onClose();
              }}
              className={Style.removeBtn}
            >
              <IconTrashLarge color="#777" />
            </button>
          </div>
        ))}
        <Button
          text="Сохранить состояние"
          onClick={() => {
            updateImages(newImages);
            onClose();
          }}
          color="#2e67ea"
        />
      </div>
    </PopUp>
  );
};

export default ChangeImagesPopUp;
