import { FC } from 'react';

import Style from './Skeleton.module.css';

type SkeletonColor = 'light' | 'dark' | 'light-grey';

export interface SkeletonProps {
  color?: SkeletonColor;
}

const Skeleton: FC<SkeletonProps> = ({ color = 'light' }) => {
  let backgroundColor = '';

  if (color === 'light') {
    backgroundColor = '#eee';
  } else if (color === 'dark') {
    backgroundColor = '#777';
  } else if (color === 'light-grey') {
    backgroundColor = '#aaa';
  }

  const skeletonStyle = {
    backgroundColor: backgroundColor,
  };

  return <div className={Style.skeleton} style={skeletonStyle}></div>;
};

export default Skeleton;
