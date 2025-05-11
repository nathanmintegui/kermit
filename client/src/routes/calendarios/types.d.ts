type CalendarioResponse = {
	competencias: Competencias;
	legenda: Legenda;
};

type Competencias = {
	competencia: Array<Competencia>;
};

type Competencia = {
	mes: string;
	dias: Array<Data>;
};

type Data = {
	data: Date;
};

type TLegenda = {
	itemsLegenda: Array<ItemLengenda>;
};

export type TItemLengenda = {
	id: number;
	cor: string;
	nome: string;
	datas: Array<Date>;
};
