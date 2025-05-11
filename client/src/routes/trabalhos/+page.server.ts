import { mockRequest } from '$lib/utils/http-client.local';
import type { PageServerLoad } from './$types';

type TTrabalhosResponse = {
	data: Array<TTrabalho>;
}

type TTrabalho = {
	id: number;
	nome: string;
	emAndamento: boolean;
	totalGrupos: number;
}

export const load: PageServerLoad = async () => {
	const response = await mockRequest<TTrabalhosResponse>(
		{
			data: [
				{ id: 1, nome: 'Trabalho 1', emAndamento: false, totalGrupos: 6 },
				{ id: 2, nome: 'Trabalho 2', emAndamento: false, totalGrupos: 11 },
				{ id: 3, nome: 'Trabalho 3', emAndamento: false, totalGrupos: 9 },
				{ id: 4, nome: 'Trabalho 4', emAndamento: true, totalGrupos: 5 }
			]
		},
		{ status: 200 }
	);

	return {
		trabalhos: response.data
	};
};
