import type { Action, PageServerLoad } from './$types';

const HOST = 'http://localhost:5201';
let BASE_URI = `${HOST}/v1/calendarios`;

export const load: PageServerLoad = async ({ fetch, url }) => {
	if (url.searchParams.get('trilha') === 'geral' || url.searchParams.get('trilha') === null) {
		BASE_URI = `${HOST}/v1/calendarios`;
	} else {
		BASE_URI = `${HOST}/v1/calendarios/${url.searchParams.get('trilha')}`;
	}

	const res = await fetch(BASE_URI);
	const data = await res.json();

	BASE_URI = `${HOST}/v1/calendarios`;
	const response = await fetch(`${BASE_URI}/info-cadastro`);

	const trilhas = await response.json();

	BASE_URI = `${HOST}/v1/calendarios`;

	return {
		calendario: data,
		trilhas: trilhas.trilhas
	};
};

export const actions = {
	addEvent: async ({ request }) => {
		const formData = await request.formData();

		const body = {
			conteudoProgramatico: formData.get('evento'),
			datas: JSON.parse(formData.get('dias')),
			cor: '#FFFFFF'
		};

		const UUID = '5353aedc-c178-4677-a9dd-53cb2644a078';
		const uri = `${BASE_URI}/${UUID}/conteudo-programatico`;

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
