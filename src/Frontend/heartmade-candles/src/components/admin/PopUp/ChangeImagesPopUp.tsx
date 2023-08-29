import { FC, useState, DragEvent } from 'react';

import { PopUpProps } from './PopUp';
import Button from '../../shared/Button';
import { Image } from '../../../types/Image';
import IconRemoveLarge from '../../../UI/IconRemoveLarge';
import IconTrashLarge from '../../../UI/IconTrashLarge';

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

  const urlToImage = 'http://localhost:5000/StaticFiles/Images/';

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
    console.log(newStateImages);
  }

  return (
    <div className={Style.overlay}>
      <div className={Style.popUp}>
        <button className={Style.closeButton} onClick={onClose}>
          <IconRemoveLarge color="#777" />
        </button>
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
              <div className={Style.image}>
                <img src={urlToImage + image.fileName} alt={image.alternativeName} />
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
      </div>
    </div>
  );
};

export default ChangeImagesPopUp;
