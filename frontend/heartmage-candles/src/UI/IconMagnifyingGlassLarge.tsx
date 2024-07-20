import React from 'react';
import { IconProps } from './IconProps';

const IconMagnifyingGlassLarge: React.FC<IconProps> = ({ color = '#000' }) => {
  return (
    <svg
      width="24"
      height="24"
      viewBox="0 0 24 24"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
    >
      <path
        fillRule="evenodd"
        clipRule="evenodd"
        d="M10.875 4C7.07804 4 4 7.07804 4 10.875C4 14.672 7.07804 17.75 10.875 17.75C14.672 17.75 17.75 14.672 17.75 10.875C17.75 7.07804 14.672 4 10.875 4ZM2 10.875C2 5.97347 5.97347 2 10.875 2C15.7765 2 19.75 5.97347 19.75 10.875C19.75 12.9656 19.0271 14.8874 17.8177 16.4041L21.7066 20.293C22.0971 20.6835 22.0971 21.3167 21.7066 21.7072C21.316 22.0977 20.6829 22.0977 20.2923 21.7072L16.4034 17.8182C14.8868 19.0273 12.9653 19.75 10.875 19.75C5.97347 19.75 2 15.7765 2 10.875Z"
        fill={color}
      />
    </svg>
  );
};

export default IconMagnifyingGlassLarge;
