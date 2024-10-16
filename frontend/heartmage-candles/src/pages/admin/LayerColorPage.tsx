import { FC, useState, useContext } from 'react';
import { useParams } from 'react-router-dom';

import MainInfoLayerColor from '../../modules/admin/MainInfoLayerColor';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import useLayerColorByIdQuery from '../../hooks/admin/useLayerColorByIdQuery';
import { AuthContext } from '../../contexts/AuthContext';
import MainInfoSkeleton from '../../modules/admin/MainInfoSkeleton';

import Style from './LayerColorPage.module.css';

type LayerColorParams = {
  id: string;
};

const LayerColorPage: FC = () => {
  const { id } = useParams<LayerColorParams>();
  const { isAuth } = useContext(AuthContext);
  const { data, isLoading, updateLayerColor } = useLayerColorByIdQuery(
    id,
    isAuth,
  );
  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  if (isLoading) {
    return <MainInfoSkeleton />;
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
