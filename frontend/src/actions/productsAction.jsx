import {createAsyncThunk} from '@reduxjs/toolkit'; 
import axios from '../utilities/axios'; 
import { delayedTimeout } from '../utilities/delayedTimeout';

export const getProducts  = createAsyncThunk (
    "products/getProducts", // <--- END POINT

    // OBTENER LOS PRODUCTOS DEL BACKEND
    async (ThunkApi, {rejectWithValue}) => {
        try{
            //Delay de un segundo
            await delayedTimeout(1000); 

            // LLAMADA PARA LOS PRODUCTOS
            return await axios.get(`/api/v1/product/list`); 
        }catch(err){
            return rejectWithValue(`Errores: ${err.message}`);
        }
    }

)