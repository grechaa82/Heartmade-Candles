import { ChangeEvent, FC, useState } from 'react';

import IconCheckLarge from '../../UI/IconCheckLarge';

import Style from './Checkbox.module.css';

interface CheckboxProps {
  checked?: boolean;
  onChange?: (isActive: boolean) => void;
}

const Checkbox: FC<CheckboxProps> = ({ checked = false, onChange }) => {
  const [isChecked, setIsChecked] = useState<boolean>(checked);

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    const { checked } = event.target;
    setIsChecked(checked);
    if (onChange) {
      onChange(checked);
    }
  };

  return (
    <label className={Style.checkboxLabel}>
      <input
        type="checkbox"
        className={Style.checkbox}
        checked={isChecked}
        onChange={handleChange}
        placeholder=""
      />
      <div className={Style.fakeCheckbox}>
        <div className={isChecked ? Style.checked : ''}></div>
        <IconCheckLarge color="#fff" />
      </div>
    </label>
  );
};

export default Checkbox;
