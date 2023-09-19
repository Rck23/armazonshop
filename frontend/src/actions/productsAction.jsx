import {createAsyncThunk} from '@reduxjs/toolkit'; 
import axios from '../utilities/axios'; 

export const getProducts  = createAsyncThunk (
    "products/getProducts", // <--- END POINT

    // OBTENER LOS PRODUCTOS DEL BACKEND
    async (ThunkApi, {rejectWithValue}) => {
        try{
            // LLAMADA PARA LOS PRODUCTOS
            return await axios.get(`/api/v1/product/list`); 
        }catch(err){
            return rejectWithValue(`Errores: ${err.message}`);
        }
    }

)