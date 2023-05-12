import React from 'react';

interface IconCheckLargeProps {
  color?: string;
}

const IconCheckLarge: React.FC<IconCheckLargeProps> = ({ color = '#000' }) => {
  return (
    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
      <path
        fill-rule="evenodd"
        clip-rule="evenodd"
        d="M19.7304 6.31701C20.1076 6.72041 20.0864 7.35322 19.683 7.73043L10.058 16.7304C9.67363 17.0898 9.07649 17.0899 8.69209 16.7305L4.31709 12.6405C3.91365 12.2633 3.89234 11.6305 4.2695 11.2271C4.64667 10.8236 5.27947 10.8023 5.68292 11.1795L9.37493 14.631L18.317 6.26958C18.7204 5.89237 19.3532 5.91361 19.7304 6.31701Z"
        fill={color}
      />
    </svg>
  );
};

export default IconCheckLarge;
