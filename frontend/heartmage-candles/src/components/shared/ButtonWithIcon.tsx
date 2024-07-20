import { FC } from 'react';
import { IconProps } from '../../UI/IconProps';
import { ButtonProps } from './Button';
import Style from './ButtonWithIcon.module.css';
import StyleButton from './Button.module.css';

interface ButtonWithIconProps extends ButtonProps {
  icon: React.FC<IconProps>;
  onClick: () => void;
}

const ButtonWithIcon: FC<ButtonWithIconProps> = ({
  icon: Icon,
  onClick,
  ...buttonProps
}) => {
  const buttonStyle = {
    color: buttonProps.color,
    ...(buttonProps.height && { height: `${buttonProps.height - 4}px` }),
    ...(buttonProps.width && { width: `${buttonProps.width}px` }),
  };

  const buttonSizeClass = `buttonSize_${buttonProps.size}`;
  const textSizeClass = `textSize_${buttonProps.size}`;

  return (
    <button
      className={`${StyleButton.button} ${Style.buttonButtonWithIcon} ${StyleButton[buttonSizeClass]}`}
      onClick={onClick}
      style={buttonStyle}
      type="button"
    >
      <Icon color={buttonProps.color} />
      <p className={StyleButton[textSizeClass]}>{buttonProps.text}</p>
    </button>
  );
};

export default ButtonWithIcon;
