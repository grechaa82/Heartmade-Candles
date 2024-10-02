import { FC, useState } from 'react';
import { useParams } from 'react-router-dom';

import MainInfoLayerColor from '../../modules/admin/MainInfoLayerColor';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import useLayerColorByIdQuery from '../../hooks/useLayerColorByIdQuery';

import Style from './LayerColorPage.module.css';

type LayerColorParams = {
  id: string;
};

const LayerColorPage: FC = () => {
  const { id } = useParams<LayerColorParams>();
  const { data, isLoading, updateLayerColor } = useLayerColorByIdQuery(id);
  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  if (isLoading) {
    return <div>...Loading</div>;
  }

  return (
    <>
      <div className="layerColors">
        {data && <MainInfoLayerColor data={data} onSave={updateLayerColor} />}
      </div>
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default LayerColorPage;
