import { FC } from 'react';

import Skeleton from '../../components/shared/skeleton';

import Style from './TagsGridSkeleton.module.css';

const TagsGridSkeleton: FC = ({}) => {
  const countTagsSkeleton = [0, 0, 0, 0, 0, 0];

  return (
    <div className={Style.tabsInputSkeleton}>
      <div className={Style.titleSkeleton}>
        <Skeleton />
      </div>
      <div className={Style.inputSkeleton}>
        {countTagsSkeleton.map((_, index) => {
          return (
            <div className={Style.tagSkeleton} key={index}>
              <Skeleton color="light-grey" />
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default TagsGridSkeleton;
