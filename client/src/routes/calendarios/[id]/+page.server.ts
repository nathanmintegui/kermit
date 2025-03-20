import type { PageLoad } from '../../../../.svelte-kit/types/src/routes';
import { mockRequest } from '$lib/utils/http-client.local';
import type { Action } from './$types';

export const load: PageLoad = async ({ fetch, params }) => {
	//console.log('ID da url ', params?.id);

	const host = 'http://localhost:5201';

	const res = await fetch(`${host}/v1/calendarios`);
	const data = await res.json();

	const response = await mockRequest(
		{
			data: [
				{ id: 1, trilha: 'Geral' },
				{ id: 2, trilha: 'FullStack' },
				{ id: 3, trilha: 'Dados' },
				{ id: 4, trilha: 'QA' }
			]
		},
		{ status: 200 }
	);

	const trilhas = await response.json();

	return {
		calendario: data,
		trilhas: trilhas.data
	};
};

export const actions = {
	addEvent: async ({ request }) => {
		const HOST = 'http://localhost:5201';

		const formData = await request.formData();

		const body = {
			conteudoProgramatico : formData.get('evento'),
			datas: JSON.parse(formData.get('dias')),
			cor: '#FFFFFF'
		};

		const UUID = "5353aedc-c178-4677-a9dd-53cb2644a078";
		const uri = `${HOST}/v1/calendarios/${UUID}/conteudo-programatico`;
		const res = await fetch(uri, {
			method: 'POST',
			body: JSON.stringify(body),
			headers: {
				'Content-Type': 'application/json'
			}
		});

		if (!res.ok) {
			console.error(`Error processing POST request to ${uri} | response: `, res);
		}

		return { success: true };
	}
} satisfies Action;
