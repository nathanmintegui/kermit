import { error, fail } from '@sveltejs/kit';
import type { Actions, PageServerLoad } from './$types';

interface Data {
	trilhas: Trilha[];
	edicoes: Edicao[];
}

interface Trilha {
	id: number;
	nome: string;
}

interface Edicao {
	id: number;
	nome: string;
}

interface Competencia {
	valor: string;
	competencias: ItemCompetencia[];
}

interface ItemCompetencia {
	mes: number;
	ano: number;
}

export const load: PageServerLoad = async () => {
	const host = 'http://localhost:5201';

	const res = await fetch(`${host}/v1/calendarios/info-cadastro`);
	const data: Data = await res.json();

	if (data) {
		return {
			trilhas: data?.trilhas,
			edicoes: data?.edicoes
		};
	}

	error(404, 'Não encontrado');
};

export const actions = {
	default: async ({ request }) => {
		const data = await request.formData();
		const edicao = data.get('edicao');
		const trilhas = data.getAll('trilhas');

		const competencias: Competencia[] = trilhas.map((t) => {
			return {
				valor: t as unknown as string,
				competencias: data.getAll(`competencias-${t as unknown as string}`).map((c) => {
					return { mes: JSON.parse(c)?.mes, ano: JSON.parse(c)?.ano };
				})
			};
		});

		if (!edicao) {
			return fail(422, {
				description: data.get('edicao'),
				error: 'Preencha este campo'
			});
		}

		const body = {
			edicao: edicao,
			trilhas: competencias
		};

		const host = 'http://localhost:5201';
		const res = await fetch(`${host}/v1/calendarios`, {
			method: 'POST',
			body: body
		});
		if (res.status === 500) {
			error(500, {
				message: 'Internal Server Error'
			});
		}
	}
} satisfies Actions;
