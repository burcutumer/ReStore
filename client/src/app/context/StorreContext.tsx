import { createContext, PropsWithChildren, useContext, useState } from "react";
import { Basket } from "../models/basket";

interface StoreContextValue {
    basket: Basket | null;
    setBasket: (basket: Basket) => void;
    removeItem: (productId: number, quantity: number) => void;
}
//StoreCoontext  basket setBasket removeItem
export const StoreContext = createContext<StoreContextValue|undefined>(undefined);

//react hooks always starts with use
export function useStoreContext(){
    const context = useContext(StoreContext);

    if(context === undefined){
        throw Error('Oops- we do not seem to be inside the provider');
    }
    
    return context;
}

export function StoreProvider({children}: PropsWithChildren<any>)
{                         //usestate hook
    const[basket,setBasket] = useState<Basket|null>(null);
    
    function removeItem(productId:number, quantity:number){
        if(!basket) return;

        const items= [...basket.items];
        const itemIndex = items.findIndex(i => i.productId === productId);
        if(itemIndex >= 0){
            items[itemIndex].quantity -= quantity;
            if(items[itemIndex].quantity === 0) items.splice(itemIndex, 1);
            setBasket(prevState => {
                return {...prevState!, items}
            })
        }
    }

    return (  //what u provide to your children
        <StoreContext.Provider value={{basket,setBasket,removeItem}}>
            {children}
        </StoreContext.Provider>
    )
}