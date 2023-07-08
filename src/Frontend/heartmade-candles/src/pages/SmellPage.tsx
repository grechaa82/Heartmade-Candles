import { FC, useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import MainInfoSmell from "../modules/MainInfoSmell";
import { Smell } from "../types/Smell";
import { SmellRequest } from "../types/Requests/SmellRequest";

import { SmellsApi } from "../services/SmellsApi";

type SmellParams = {
  id: string;
};

const SmellPage: FC = () => {
  const { id } = useParams<SmellParams>();
  const [smellData, setSmellData] = useState<Smell>();

  const handleChangesSmell = (updatedSmell: Smell) => {
    setSmellData((prevSmellData) => ({
      ...prevSmellData,
      ...updatedSmell,
    }));
  };

  const updateSmell = async (updatedItem: Smell) => {
    if (id) {
      const smellRequest: SmellRequest = {
        title: updatedItem.title,
        description: updatedItem.description,
        price: updatedItem.price,
        isActive: updatedItem.isActive,
      };
      await SmellsApi.update(id, smellRequest);
    }
  };

  useEffect(() => {
    async function fetchSmell() {
      if (id) {
        const data = await SmellsApi.getById(id);
        setSmellData(data);
      }
    }

    fetchSmell();
  }, [id]);

  return (
    <>
      <div className="smells">
        {smellData && (
          <MainInfoSmell
            data={smellData}
            handleChangesSmell={handleChangesSmell}
            onSave={updateSmell}
          />
        )}
      </div>
    </>
  );
};

export default SmellPage;
