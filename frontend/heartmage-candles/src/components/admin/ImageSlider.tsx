import { FC, useState } from 'react';

import IconChevronLeftLarge from '../../UI/IconChevronLeftLarge';
import IconChevronRightLarge from '../../UI/IconChevronRightLarge';
import IconDownloadLarge from '../../UI/IconDownloadLarge';
import IconViewListLarge from '../../UI/IconViewListLarge';
import { Image } from '../../types/Image';
import AddImagesPopUp from './PopUp/AddImagesPopUp';
import ChangeImagesPopUp from './PopUp/ChangeImagesPopUp';
import Picture, { SourceSettings } from '../shared/Picture';

import { ImagesApi } from '../../services/ImagesApi';

import Style from './ImageSlider.module.css';

interface ImageSliderProps {
  images: Image[];
  updateImages: (images: Image[]) => void;
  addImages: (images: Image[]) => void;
}

const ImageSlider: FC<ImageSliderProps> = ({
  images,
  updateImages,
  addImages,
}) => {
  const [currentImageIndex, setCurrentImageIndex] = useState(0);
  const [isAddImagesPopUpOpen, setIsAddImagesPopUpOpen] = useState(false);
  const [isChangeImagesPopUpOpen, setIsChangeImagesPopUpOpen] = useState(false);

  const handleChangeImage = (index: number, moveToIndex?: number) => {
    let current = index;
    if (moveToIndex && moveToIndex == -1) {
      current -= 1;
      if (current < 0) {
        current = images.length - 1;
      }
    } else if (moveToIndex && moveToIndex == 1) {
      current += 1;
      if (current >= images.length) {
        current = 0;
      }
    }
    setCurrentImageIndex(current);
  };

  const handleAddImagesPopUpOpen = () => {
    setIsAddImagesPopUpOpen(true);
  };

  const handleAddImagesPopUpClose = async (uploadedImages?: string[]) => {
    if (uploadedImages) {
      const imagesToDelete = uploadedImages;
      await ImagesApi.deleteImages(imagesToDelete);
    }
    setIsAddImagesPopUpOpen(false);
  };

  const handleChangeImagesPopUpOpen = () => {
    setIsChangeImagesPopUpOpen(true);
  };

  const handleChangeImagesPopUpClose = () => {
    setIsChangeImagesPopUpOpen(false);
  };

  const handleDeleteImage = async (image: Image) => {
    let fileNames: string[] = [];
    fileNames.push(image.fileName);
    const imagesResponse = await ImagesApi.deleteImages(fileNames);
    if (!imagesResponse.error) {
      const newImagesState: Image[] = images.filter(
        (i) => i.fileName !== image.fileName,
      );
      updateImages(newImagesState);
    }
  };

  const handleUploadImage = async (files: File[]) => {
    const imagesResponse = await ImagesApi.uploadImages(files);
    if (imagesResponse.data && !imagesResponse.error) {
      return imagesResponse.data;
    } else {
      return [];
    }
  };

  if (images.length === 0) {
    return (
      <div className={Style.imageSlider}>
        <div className={Style.changesImages}>
          <button
            className={Style.iconUploadImages}
            onClick={handleAddImagesPopUpOpen}
          >
            <IconDownloadLarge color="#6FCF97" />
          </button>
        </div>
        <div className={Style.imageNoAvailable}>Изображение отсутствует</div>
        {isAddImagesPopUpOpen && (
          <AddImagesPopUp
            onClose={handleAddImagesPopUpClose}
            uploadImages={handleUploadImage}
            updateImages={addImages}
          />
        )}
      </div>
    );
  }

  const sourceSettingsForMainImage: SourceSettings[] = [
    {
      size: 'small',
      media: '(max-width: 200px)',
    },
    {
      size: 'medium',
      media: '(max-width: 632px)',
    },
    {
      size: 'large',
      media: '(max-width: 768px)',
    },
    {
      size: 'medium',
      media: '(max-width: 1525px)',
    },
    {
      size: 'large',
      media: '(min-width: 1525px)',
    },
  ];

  return (
    <div className={Style.imageSlider}>
      <Picture
        name={images[currentImageIndex].fileName}
        alt={images[currentImageIndex].alternativeName}
        className={Style.squareImage}
        sourceSettings={sourceSettingsForMainImage}
      />
      <div className={Style.slider}>
        <button
          className={Style.iconChevronBtn}
          onClick={() => handleChangeImage(currentImageIndex, -1)}
          type="button"
        >
          <IconChevronLeftLarge color="#777" />
        </button>
        {images.map((image, index) => (
          <button
            className={Style.sliderBtn}
            type="button"
            onClick={() => handleChangeImage(index)}
            key={index}
          >
            <Picture
              name={image.fileName}
              alt={image.alternativeName}
              sourceSettings={[
                {
                  size: 'small',
                },
              ]}
            />
          </button>
        ))}
        <button
          className={Style.iconChevronBtn}
          onClick={() => handleChangeImage(currentImageIndex, 1)}
          type="button"
        >
          <IconChevronRightLarge color="#777" />
        </button>
      </div>
      <div className={Style.changesImages}>
        <button
          className={Style.iconChangeImages}
          onClick={handleChangeImagesPopUpOpen}
        >
          <IconViewListLarge color="#6FCF97" />
        </button>
        <button
          className={Style.iconUploadImages}
          onClick={handleAddImagesPopUpOpen}
        >
          <IconDownloadLarge color="#6FCF97" />
        </button>
      </div>
      {isAddImagesPopUpOpen && (
        <AddImagesPopUp
          onClose={handleAddImagesPopUpClose}
          uploadImages={handleUploadImage}
          updateImages={addImages}
        />
      )}
      {isChangeImagesPopUpOpen && (
        <ChangeImagesPopUp
          images={images}
          onClose={handleChangeImagesPopUpClose}
          onRemove={handleDeleteImage}
          updateImages={updateImages}
        />
      )}
    </div>
  );
};

export default ImageSlider;
