import { FC, ChangeEvent, DragEvent, useState } from 'react';

import Style from './ImageUploader.module.css';

export interface ImageUploaderProps {
  uploadImages: (files: File[]) => Promise<void>;
}

const ImageUploader: FC<ImageUploaderProps> = ({ uploadImages }) => {
  const [dragging, setDragging] = useState(false);

  const handleDragOver = (event: DragEvent<HTMLDivElement>) => {
    event.preventDefault();
    setDragging(true);
  };

  const handleDragLeave = () => {
    setDragging(false);
  };

  const handleDrop = (event: DragEvent<HTMLDivElement>) => {
    event.preventDefault();
    setDragging(false);
    processFiles(event.dataTransfer.files);
  };

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    if (event.target.files) {
      processFiles(event.target.files);
    }
  };

  const processFiles = (files: FileList) => {
    const imageFiles = Array.from(files).filter((file) =>
      file.type.startsWith('image/'),
    );
    if (imageFiles.length > 0) {
      uploadImages(imageFiles);
    }
  };

  return (
    <div
      className={`${Style.uploadImageBlock} ${dragging ? Style.dragging : ''}`}
      onDragOver={handleDragOver}
      onDragLeave={handleDragLeave}
      onDrop={handleDrop}
    >
      <label htmlFor="fileInput" className={Style.labelInput}>
        <span>Выберите файл</span> <br /> или перетащите его сюда
      </label>
      <input
        className={Style.input}
        id="fileInput"
        type="file"
        accept="image/*"
        multiple
        onChange={handleChange}
      />
    </div>
  );
};

export default ImageUploader;
