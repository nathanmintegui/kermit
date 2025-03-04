<script lang="ts">
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();

	const grupos = data?.grupos;
</script>

<div class="page-container flex flex-col items-center min-h-[100%] prevent-select">
	<h1 class="bg-blue-300 p-4 w-screen text-center">Grupos {grupos.nome}</h1>

	<div class="flex gap-12 pt-12">
		{#each grupos.grupos as grupo}
			<div class="rounded-sm flex flex-col items-center p-4 over:cursor-pointer bg-white">
				<p class="bg-amber-200">{grupo.nome}</p>
				<div>
					{#each grupo.integrantes as integrante}
						<div>
							<p class="{integrante.cargo === 'TL' && 'font-bold'} hover:underline">{integrante.nome}</p>
						</div>
					{/each}
				</div>
			</div>
		{/each}
	</div>
</div>

<style>
    .page-container {
        /*
				background-color: #e5e5f7;
				opacity: 0.4;
				background-image: linear-gradient(#adafde 0.6000000000000001px, transparent 0.6000000000000001px), linear-gradient(to right, #adafde 0.6000000000000001px, #e5e5f7 0.6000000000000001px);
				background-size: 12px 12px;
				 */

        /*
				background-color: #e5e5f7;
				background-image: radial-gradient(#adafde 1px, #e5e5f7 1px);
				background-size: 20px 20px;
         */

        background-color: #e5e5f7;
        background: radial-gradient(circle, transparent 20%, #e5e5f7 20%, #e5e5f7 80%, transparent 80%, transparent), radial-gradient(circle, transparent 20%, #e5e5f7 20%, #e5e5f7 80%, transparent 80%, transparent) 10px 10px, linear-gradient(#adafde 0.8px, transparent 0.8px) 0 -0.4px, linear-gradient(90deg, #adafde 0.8px, #e5e5f7 0.8px) -0.4px 0;
        background-size: 20px 20px, 20px 20px, 10px 10px, 10px 10px;
    }
</style>