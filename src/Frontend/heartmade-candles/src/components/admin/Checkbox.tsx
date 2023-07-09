import { ChangeEvent, FC, useState } from "react";
import IconCheckLarge from "../../UI/IconCheckLarge";
import Style from "./Checkbox.module.css";

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
    <label className={Style.checkbox}>
      <input type="checkbox" checked={isChecked} onChange={handleChange} />
      <div className={Style.checkboxIcon}>
        <IconCheckLarge color="#fff" />
      </div>
    </label>
  );
};

export default Checkbox;
