import { FC, useState } from "react";

import IconChevronLeftLarge from "../../UI/IconChevronLeftLarge";
import IconChevronRightLarge from "../../UI/IconChevronRightLarge";
import IconDownloadLarge from "../../UI/IconDownloadLarge";
import IconViewListLarge from "../../UI/IconViewListLarge";
import { Image } from "../../types/Image";
import AddImagesPopUp from "./PopUp/AddImagesPopUp";

import { ImagesApi } from "../../services/ImagesApi";

import Style from "./ImageSlider.module.css";

interface ImageSliderProps {
  images: Image[];
  updateImages: (images: Image[]) => void;
}

const ImageSlider: FC<ImageSliderProps> = ({ images, updateImages }) => {
  const [currentImageIndex, setCurrentImageIndex] = useState(0);
  const urlToImage = "http://localhost:5000/StaticFiles/Images/";
  const [isPopUpOpen, setIsPopUpOpen] = useState(false);

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

  const handlePopUpOpen = () => {
    setIsPopUpOpen(true);
  };

  const handlePopUpClose = () => {
    setIsPopUpOpen(false);
  };

  if (images.length === 0) {
    return (
      <div className={Style.image}>
        <div className={Style.imageNoAvailable}>Изображение отсутствует</div>
        <div className={Style.changesImages}>
          <button className={Style.iconChangeImages} onClick={handlePopUpOpen}>
            <IconViewListLarge color="#6FCF97" />
          </button>
          <button className={Style.iconUploadImages} onClick={handlePopUpOpen}>
            <IconDownloadLarge color="#6FCF97" />
          </button>
        </div>
        {isPopUpOpen && (
          <AddImagesPopUp
            onClose={handlePopUpClose}
            uploadImages={ImagesApi.uploadImages}
            updateImages={updateImages}
          />
        )}
      </div>
    );
  }

  return (
    <div className={Style.image}>
      <img
        src={urlToImage + images[currentImageIndex].fileName}
        alt={images[currentImageIndex].alternativeName}
      />
      <div className={Style.slider}>
        <button
          className={Style.iconChevronBtn}
          onClick={() => handleChangeImage(currentImageIndex, -1)}
        >
          <IconChevronLeftLarge color="#777" />
        </button>
        {images.map((image, index) => (
          <img
            key={index}
            src={urlToImage + image.fileName}
            className={Style.sliderImage}
            onClick={() => handleChangeImage(index)}
          />
        ))}
        <button
          className={Style.iconChevronBtn}
          onClick={() => handleChangeImage(currentImageIndex, 1)}
        >
          <IconChevronRightLarge color="#777" />
        </button>
      </div>
      <div className={Style.changesImages}>
        <button className={Style.iconChangeImages} onClick={handlePopUpOpen}>
          <IconViewListLarge color="#6FCF97" />
        </button>
        <button className={Style.iconUploadImages} onClick={handlePopUpOpen}>
          <IconDownloadLarge color="#6FCF97" />
        </button>
      </div>
      {isPopUpOpen && (
        <AddImagesPopUp
          onClose={handlePopUpClose}
          uploadImages={ImagesApi.uploadImages}
          updateImages={updateImages}
        />
      )}
    </div>
  );
};

export default ImageSlider;
