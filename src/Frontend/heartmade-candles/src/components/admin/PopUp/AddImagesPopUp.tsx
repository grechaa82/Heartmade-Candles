import { FC, useState } from 'react';

import PopUp, { PopUpProps } from './PopUp';
import ButtonWithIcon from '../ButtonWithIcon';
import IconPlusLarge from '../../../UI/IconPlusLarge';
import { Image } from '../../../types/Image';

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
      console.log('AddImagesPopUp handleFileChange', newFiles, newFileNames);
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
    <PopUp onClose={onClose}>
      <div className={Style.cont}>
        <input className={Style.inputImage} type="file" accept="*/*" onChange={handleFileChange} />
        <ButtonWithIcon
          text="Добавить фотографию"
          icon={IconPlusLarge}
          onClick={() => {
            updateImages(images);
            onClose();
          }}
        />
      </div>
    </PopUp>
  );
};

export default AddImagesPopUp;
