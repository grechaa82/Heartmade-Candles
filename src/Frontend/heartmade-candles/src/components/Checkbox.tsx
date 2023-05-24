import React, { useState } from 'react';
import IconCheckLarge from '../UI/IconCheckLarge';
import Style from './Checkbox.module.css';

interface CheckboxProps {
  checked?: boolean;
}

const Checkbox: React.FC<CheckboxProps> = ({ checked = false }) => {
  const [isChecked, setIsChecked] = useState<boolean>(checked);

  const handleCheckboxChange = () => {
    setIsChecked(!isChecked);
  };

  return (
    <label className={Style.checkbox}>
      <input type="checkbox" checked={isChecked} onChange={handleCheckboxChange} />
      <div className={Style.checkboxIcon}>
        <IconCheckLarge color="#fff" />
      </div>
    </label>
  );
};

export default Checkbox;
