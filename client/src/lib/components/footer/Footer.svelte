<script lang="ts">
	let { sessaoAtual, trilhas } = $props();
</script>

<div class="w-screen border-t border-t-gray-300 absolute bottom-0 flex justify-end gap-5 bg-white">
	<div class="flex gap-5 p-1">
		{#each trilhas as trilha}
			<a
				href="/calendarios?trilha={trilha?.calendarioId}"
				rel="external"
				class="hover:cursor-pointer hover:font-bold pr-3 {trilha.trilha === sessaoAtual &&
				'font-bold text-blue-600'}"
			>
				{trilha.trilha}
			</a>
		{/each}
	</div>
</div>
