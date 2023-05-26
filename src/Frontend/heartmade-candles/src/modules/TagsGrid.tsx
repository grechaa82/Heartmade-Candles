import React, { FC } from 'react';
import { NumberOfLayer } from '../types/NumberOfLayer';
import { TypeCandle } from '../types/TypeCandle';
import InputTag from '../components/InputTag';
import { TagProps } from '../components/Tag';
import Tag from '../components/Tag';

import Style from './TagsGrid.module.css';

type TagsData = TypeCandle[] | NumberOfLayer[];

export interface TagsGridProps {
  data: TagsData;
  title: string;
}

const TagsGrid: FC<TagsGridProps> = ({ data, title }) => {
  const tagsData: TagProps[] = data.map((item) => ({
    id: item.id,
    text: 'number' in item ? `${item.number}` : `${item.title}`,
    removable: true,
  }));

  return (
    <div className={Style.tabsInput}>
      <h2>{title}</h2>
      <InputTag tagsData={tagsData} />
    </div>
  );
};

export default TagsGrid;
