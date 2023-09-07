import { ChangeEvent, FC, useState } from 'react';
import Checkbox from './Checkbox';

import Style from './CheckboxBlock.module.css';

interface CheckboxBlockProps {
  text: string;
  color?: string;
  checked: boolean;
  onChange?: (isActive: boolean) => void;
}

const CheckboxBlock: FC<CheckboxBlockProps> = ({ text, color = '#222', checked, onChange }) => {
  const [isChecked, setIsChecked] = useState<boolean>(checked);

  const CheckboxBlockStyle = {
    color: color,
  };

  return (
    <div className={Style.checkboxBlock}>
      <p style={CheckboxBlockStyle}>{text}</p>
      <Checkbox checked={isChecked} onChange={onChange} />
    </div>
  );
};

export default CheckboxBlock;
