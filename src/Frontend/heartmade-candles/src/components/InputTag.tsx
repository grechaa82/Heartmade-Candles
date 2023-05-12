import React, { FC } from 'react';
import IconRemoveLarge from '../UI/IconRemoveLarge';

import Style from './InputTag.module.css';

interface InputTagProps {
  text: string;
}

const InputTag: React.FC<InputTagProps> = ({ text }) => {
  return (
    <div className={Style.inputTag}>
      <p>{text}</p>
      <IconRemoveLarge color="#fff" />
    </div>
  );
};

export default InputTag;
